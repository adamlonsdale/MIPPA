import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { PlayerMatchViewModel } from '../viewmodel/scorecard/playermatchviewmodel';

import { ScorecardService } from './scorecard.service';

@Component({
    selector: 'player-match',
    template: require('./player-match.component.html'),
    styles: [require('./scorecard.component.css')]
})
export class PlayerMatchComponent implements OnInit {
    @Input('myTr') viewModel: any;
    @Output() onSaveScores = new EventEmitter<boolean>();
    match: PlayerMatchViewModel;
    maxScore: number;
    spinnerMatch: boolean = false;
    scoreOptions: number[] = [];

    ngOnInit() {
        this.match = this.viewModel.match;
        this.maxScore = this.viewModel.maxScore;

        this.scoreOptions = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
    }

    constructor(private scorecardService: ScorecardService) { }

    onSelectScoreChange() {
        this.match.awayPlayerScore = this.scoreOptions[this.scoreOptions.length - 1] - this.match.homePlayerScore;
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
                }

                this.spinnerMatch = false;
            });
    }

    incrementScore(viewModel: PlayerMatchViewModel, isHome: boolean) {
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

        if (viewModel.saved) {
            viewModel.saved = false;
        }

    }

    decrementScore(viewModel: PlayerMatchViewModel, isHome: boolean) {
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

        if (viewModel.saved) {
            viewModel.saved = false;
        }
    }
}

class ScoreValidationViewModel {
    homeScore: number;
    awayScore: number;
    scorecardState: number;
}