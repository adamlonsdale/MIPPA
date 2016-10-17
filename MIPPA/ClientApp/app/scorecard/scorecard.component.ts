import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { Player } from '../model/player';
import { Scorecard } from '../model/scorecard';
import { Team } from '../model/team';
import { ActivatedRoute, Router } from '@angular/router';

import { ScorecardService } from './scorecard.service';
import { PlayerMatchViewModel } from '../viewmodel/scorecard/playermatchviewmodel';
import { PlayerMatchComponent } from './player-match.component';

import { Subscription } from 'RxJs/rx';

@Component({
    selector: 'scorecard-component',
    template: require('./scorecard.component.html'),
    styles: [require('./scorecard.component.css')]
})
export class ScorecardComponent implements OnInit {
    @ViewChild(PlayerMatchComponent) child: PlayerMatchComponent;
    private subscription: Subscription;

    viewModel: ScoreCardViewModel;
    lineup: Lineup;
    selectedHomePlayer: PlayerViewModel;
    selectedAwayPlayer: PlayerViewModel;
    scorecardId: number;
    scorecard: Scorecard;
    matches: Array<PlayerMatchViewModel>;
    teamResults: TeamResultsViewModel;
    requiredPlayerCount: number;
    handicapsAreNotSet: boolean;
    playersWithNoHandicap: Array<PlayerViewModel>;
    editMode: Boolean;
    enteringLineup: Boolean;

    constructor(private scorecardService: ScorecardService, private activatedRoute: ActivatedRoute, private router: Router) {
        this.viewModel = new ScoreCardViewModel();
        this.scorecardId = -1;
        this.matches = new Array<PlayerMatchViewModel>();
        this.teamResults = new TeamResultsViewModel();
        this.playersWithNoHandicap = new Array<PlayerViewModel>();
    }

    onOtherScorecard() {
        this.router.navigate(['/', 'app', 'scorecard', this.viewModel.otherScorecardId], { preserveQueryParams: true });
    }

    finalizeMatch() {
        this.scorecardService.FinalizeMatch(this.viewModel.scorecardId).subscribe(data => { }, e => console.log(e.message), () => { });
        this.viewModel.state = 3;
    }

    getTeamResults() {
        if (this.scorecardId < 0) {
            return;
        }

        this.scorecardService.GetTeamResults(this.scorecardId)
            .subscribe(
            data => {
                this.teamResults = data;
                console.log(this.child);
                if (this.child != null && this.child != undefined) {
                    this.child.scorecardState = this.teamResults.scorecardState;
                }
            },
            e => console.log(e.message),
            () => {
            });
    }

    removePlayer(indexOfPlayer: number, isHome: boolean) {
        if (isHome) {
            this.lineup.homePlayers.splice(indexOfPlayer, 1);
        }
        else {
            this.lineup.awayPlayers.splice(indexOfPlayer, 1);
        }
    }

    ngOnInit() {
        this.activatedRoute
            .queryParams
            .subscribe(queryParam => this.editMode = <Boolean>queryParam['edit']);
        this.subscription =
            this.activatedRoute.params.subscribe(
                (params: any) => {
                    this.scorecardId = +params['scorecardId'];
                    this.loadScorecard();
                }
            );
    }

    loadScorecard() {

        this.scorecardService.GetScorecardInformation(this.scorecardId)
            .subscribe(
            data => {
                this.viewModel = data;
            },
            error => console.log(error.message),
            () => {
                if (this.viewModel.matchupType == 0) {
                    this.requiredPlayerCount = 3;
                } else if (this.viewModel.matchupType == 1) {
                    this.requiredPlayerCount = 4;
                } else {
                    this.requiredPlayerCount = 5;
                }

                if (this.viewModel.state == 0) {
                    this.selectedHomePlayer = this.viewModel.homePlayers[0];
                    this.selectedAwayPlayer = this.viewModel.awayPlayers[0];
                }
                else if (this.viewModel.state != 3) {
                    this.displayMatches();
                }

                if (this.viewModel.state == 0) {
                    this.checkHandicaps();
                }

            });

        this.getTeamResults();

        this.lineup = new Lineup();
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    checkHandicaps() {
        for (let player of this.viewModel.homePlayers) {
            if (this.viewModel.format == 0 && player.handicap <= 0) {
                this.playersWithNoHandicap.push(player);
                this.handicapsAreNotSet = true;
            }
            else if (this.viewModel.format == 1 && player.handicap < -3) {
                this.playersWithNoHandicap.push(player);
                this.handicapsAreNotSet = true;
            }
        }
        for (let player of this.viewModel.awayPlayers) {
            if (this.viewModel.format == 0 && player.handicap <= 0) {
                this.playersWithNoHandicap.push(player);
                this.handicapsAreNotSet = true;
            }
            else if (this.viewModel.format == 1 && player.handicap < -3) {
                this.playersWithNoHandicap.push(player);
                this.handicapsAreNotSet = true;
            }
        }
    }

    startMatches() {
        this.enteringLineup = true;

        this.viewModel.homePlayers = this.lineup.homePlayers;
        this.viewModel.awayPlayers = this.lineup.awayPlayers;

        this.scorecardService.PostScorecardLineup(this.viewModel)
            .subscribe(
            data => { },
            e => console.log(e.message),
            () => {
                this.scorecardService.GetScorecardInformation(this.scorecardId)
                    .subscribe(
                    data => { this.viewModel = data; },
                    e => console.log(e.message),
                    () => {
                        if (this.viewModel.matchupType == 0) {
                            this.requiredPlayerCount = 3;
                        } else if (this.viewModel.matchupType == 1) {
                            this.requiredPlayerCount = 4;
                        } else {
                            this.requiredPlayerCount = 5;
                        }

                        this.displayMatches();

                        this.child.loadQueue();

                        this.enteringLineup = false;
                    });
            });
    }

    private displayMatches() {
        for (let round of this.viewModel.rounds) {
            for (let match of round.playerMatches) {
                this.matches.push(match);
            }
        }
    }

    addPlayerToRoster(home: boolean) {
        if (home) {
            if (this.selectedHomePlayer == undefined) {
                return;
            }
            this.lineup.homePlayers.push(this.selectedHomePlayer);
        }
        else {
            if (this.selectedAwayPlayer == undefined) {
                return;
            }
            this.lineup.awayPlayers.push(this.selectedAwayPlayer);
        }
    }
}

class TeamResultsViewModel {
    homeTeamTotalPoints: number;
    homeTeamTotalRounds: number;
    homeTeamHandicap: number;
    awayTeamTotalPoints: number;
    awayTeamTotalRounds: number;
    awayTeamHandicap: number;
    homeRoundsWon: Array<number>;
    awayRoundsWon: Array<number>;
    scorecardState: number;
}

class ScoreCardViewModel {
    scorecardId: number;
    homeTeamName: string;
    awayTeamName: string;
    homePlayers: Array<PlayerViewModel>;
    awayPlayers: Array<PlayerViewModel>;
    rounds: Array<RoundViewModel>;
    state: number;
    format: number;
    matchupType: number;
    otherScorecardId: number;

    constructor() {
        this.homePlayers = new Array<PlayerViewModel>();
        this.awayPlayers = new Array<PlayerViewModel>();
        this.rounds = new Array<RoundViewModel>();
    }
}

class PlayerViewModel {
    playerId: number;
    name: string;
    handicap: number;
}

class RoundViewModel {
    playerMatches: PlayerMatchViewModel[];
}

class Lineup {
    homePlayers: Array<PlayerViewModel>;
    awayPlayers: Array<PlayerViewModel>;

    constructor() {
        this.homePlayers = new Array<PlayerViewModel>();
        this.awayPlayers = new Array<PlayerViewModel>();
    }
}