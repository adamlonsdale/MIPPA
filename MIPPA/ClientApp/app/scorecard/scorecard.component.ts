import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { Player } from '../model/player';
import { Scorecard } from '../model/scorecard';
import { Team } from '../model/team';
import { ActivatedRoute, Router } from '@angular/router';

import { ScorecardService } from './scorecard.service';
import { PlayerMatchViewModel } from '../viewmodel/scorecard/playermatchviewmodel';
import { RoundComponent } from './round.component';

import { Subscription } from 'RxJs/rx';

@Component({
    selector: 'scorecard-component',
    template: require('./scorecard.component.html'),
    styles: [require('./scorecard.component.css')]
})
export class ScorecardComponent implements OnInit, OnDestroy {
    @ViewChild(RoundComponent) child: RoundComponent;
    private subscription: Subscription;
    private subscription2: Subscription;

    viewModel: ScoreCardViewModel;
    lineup: Lineup;
    selectedHomePlayer: PlayerViewModel;
    selectedAwayPlayer: PlayerViewModel;
    scorecardId: number;
    scorecard: Scorecard;
    matches: Array<PlayerMatchViewModel>;
    teamResults: TeamResultsViewModel;
    playerResults: PlayerResultsViewModel;
    requiredPlayerCount: number;
    handicapsAreNotSet: boolean;
    playersWithNoHandicap: Array<PlayerViewModel>;
    enteringLineup: Boolean;
    numberOfTables: number = 1;
    tablesSubmitted: boolean = false;

    homeTeamPlayerResultsViewModel: Array<PlayerViewModel>;
    awayTeamPlayerResultsViewModel: Array<PlayerViewModel>;

    constructor(private scorecardService: ScorecardService, private activatedRoute: ActivatedRoute, private router: Router) {
        this.viewModel = new ScoreCardViewModel();
        this.scorecardId = -1;
        this.teamResults = new TeamResultsViewModel();
        this.playerResults = new PlayerResultsViewModel();
        this.playersWithNoHandicap = new Array<PlayerViewModel>();
    }

    onRequestReset() {
        this.scorecardService.ResetScorecard(this.scorecardId).subscribe(
        data => this.loadScorecard());
    }

    onOtherScorecard() {
        this.router.navigate(['/', 'app', 'scorecard', this.viewModel.otherScorecardId]);
    }

    startLineup(something: any) {
        this.viewModel.numberOfTables = something.numberOfTables;
        this.tablesSubmitted = true;
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
                if (this.child != null && this.child != undefined) {
                    this.child.scorecardState = this.teamResults.scorecardState;
                }
            },
            e => console.log(e.message),
            () => {
            });

        this.scorecardService.GetPlayerResults(this.scorecardId)
            .subscribe(
            data => {
                this.playerResults = data;
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
                else {
                    this.displayMatches();
                }

                if (this.viewModel.state == 0) {
                    this.checkHandicaps();
                }

                this.getTeamResults();
            });

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

                        this.getTeamResults();

                        this.enteringLineup = false;
                    });
            });
    }

    private displayMatches() {
        this.matches = new Array<PlayerMatchViewModel>();

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
    homePlayerResults: Array<PlayerViewModel>;
    awayPlayerResults: Array<PlayerViewModel>;
    scorecardState: number;
}

class ScoreCardViewModel {
    scorecardId: number;
    homeTeamId: number;
    homeTeamName: string;
    awayTeamId: number;
    awayTeamName: string;
    homePlayers: Array<PlayerViewModel>;
    awayPlayers: Array<PlayerViewModel>;
    rounds: Array<RoundViewModel>;
    state: number;
    format: number;
    matchupType: number;
    otherScorecardId: number;
    numberOfTables: number;

    constructor() {
        this.homePlayers = new Array<PlayerViewModel>();
        this.awayPlayers = new Array<PlayerViewModel>();
        this.rounds = new Array<RoundViewModel>();
    }
}

class PlayerViewModel {
    playerId: number;
    name: string;
    totalScore: number;
    averageScore: number;
    handicap: number;
    wins: number;
}

class PlayerResultsViewModel {
    homePlayerResults: Array<PlayerViewModel> = [];
    awayPlayerResults: Array<PlayerViewModel> = [];
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