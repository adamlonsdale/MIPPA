import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Player } from '../model/player';
import { ScorecardService } from './scorecard.service';
import { Observable }     from 'rxjs/Observable';
import { PlayerMatchViewModel } from '../viewmodel/scorecard/playermatchviewmodel';

@Component({
    selector: 'player-match',
    template: require('./player-match.component.html'),
    styles: [require('./scorecard.component.css')]
})
export class PlayerMatchComponent implements OnInit {
    @Input() maxScore: number;
    @Input() matches: Array<PlayerMatchViewModel>;
    @Input() scorecardState: number;
    queuedMatches: Array<PlayerMatchViewModel>;
    playedMatches: Array<PlayerMatchViewModel>;
    skippedMatches: Array<PlayerMatchViewModel>;
    @Input() numberOfTables: number;
    started: Boolean;
    onDeckMatches: Array<PlayerMatchViewModel>;
    allMatches: Boolean;
    editMode: Boolean;
    @Output() onSaveScores = new EventEmitter<boolean>();
    @Output() onFinalizeMatch = new EventEmitter<boolean>();
    canSave: Boolean;
    spinnerMatch: Boolean = false;

    constructor(private scorecardService: ScorecardService, private activatedRoute: ActivatedRoute) {
        this.playedMatches = new Array<PlayerMatchViewModel>();
        this.queuedMatches = new Array<PlayerMatchViewModel>();
        this.skippedMatches = new Array<PlayerMatchViewModel>();
        this.onDeckMatches = new Array<PlayerMatchViewModel>();
    }

    ngOnInit() {
            this.activatedRoute
                .queryParams
                .subscribe(queryParam => this.editMode = <Boolean>queryParam['edit']);

        this.loadQueue();
    }

    finalizeMatch() {
        this.onFinalizeMatch.emit(true);
    }

    addTable() {
        if (this.queuedMatches.length > 0) {
            this.playedMatches.push(this.queuedMatches[0]);
            this.queuedMatches.splice(0, 1);
        }

        this.onDeck();
    }

    showAllMatches() {
        this.allMatches = !this.allMatches;
    }

    loadQueue() {
        var nonSavedMatches =
            this.matches.filter(
                function (value) {
                    return value.saved != true;
                });


        for (let match of nonSavedMatches) {
            this.queuedMatches.push(match);
        }

        this.started = true;

        for (var i = 0; i < this.numberOfTables; i++) {
            this.addTable();
        }
    }

    onDeck() {
        this.onDeckMatches = new Array<PlayerMatchViewModel>();

        for (var i = 0; i < this.numberOfTables; i++) {
            if (this.queuedMatches.length < i + 1) {
                return;
            }

            this.onDeckMatches.push(this.queuedMatches[i]);
        }
    }

    saveScores(viewModel: PlayerMatchViewModel) {
        this.spinnerMatch = true;
        var returnData = new ScoreValidationViewModel();
        viewModel.validationError = false;

        this.scorecardService.UpdatePlayerScores(viewModel)
            .subscribe(
            data => returnData = data,
            e => {
                    viewModel.validationError = true;
            },
            () => {
                if (returnData.homeScore != viewModel.homePlayerScore ||
                    returnData.awayScore != viewModel.awayPlayerScore) {
                    viewModel.validationError = true;
                }
                else {
                    viewModel.saved = true;
                    viewModel.validationError = false;
                    this.onSaveScores.emit(true);

                    if (!this.allMatches) {
                        // Get the index of the match in the queue and the index in playedTables
                        var indexOfPlayedMatch = this.playedMatches.indexOf(viewModel);
                        var indexOfQueuedMatch = this.queuedMatches.indexOf(viewModel);


                        if (this.queuedMatches.length > 0) {
                            this.playedMatches.splice(indexOfPlayedMatch, 1, this.queuedMatches[0]);
                            this.queuedMatches.splice(0, 1);
                        }
                        else {
                            this.playedMatches.splice(indexOfPlayedMatch, 1);
                        }

                        this.onDeck();
                    }
                }

                this.spinnerMatch = false;
            });
    }

    incrementScore(viewModel: PlayerMatchViewModel, isHome: boolean) {
        if (!this.editMode)
            return;

        if (isHome) {
            if (viewModel.homePlayerScore != this.maxScore) {
                viewModel.homePlayerScore++;
            }

            viewModel.awayPlayerScore = this.maxScore - viewModel.homePlayerScore;
        }
        else {
            if (viewModel.awayPlayerScore != this.maxScore) {
                viewModel.awayPlayerScore++;
            }

            viewModel.homePlayerScore = this.maxScore - viewModel.awayPlayerScore;
        }

    }

    decrementScore(viewModel: PlayerMatchViewModel, isHome: boolean) {
        if (!this.editMode)
            return;

        if (isHome) {
            if (viewModel.homePlayerScore > 0) {
                viewModel.homePlayerScore--;
            }

            viewModel.awayPlayerScore = this.maxScore - viewModel.homePlayerScore;
        }
        else {
            if (viewModel.awayPlayerScore > 0) {
                viewModel.awayPlayerScore--;
            }

            viewModel.homePlayerScore = this.maxScore - viewModel.awayPlayerScore;
        }
    }
}

class ScoreValidationViewModel {
    homeScore: number;
    awayScore: number;
    scorecardState: number;
}