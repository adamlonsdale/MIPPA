import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { Player } from '../model/player';
import { Observable }     from 'rxjs/Observable';

import { PlayerMatchViewModel } from '../viewmodel/scorecard/playermatchviewmodel';

@Component({
    selector: 'round',
    template: require('./round.component.html')
})
export class RoundComponent implements OnInit {
    @Input() maxScore: number;
    @Input() scorecardState: number;
    @Input() rounds: Array<RoundViewModel>;
    started: Boolean;
    @Output() onFinalizeMatch = new EventEmitter<boolean>();
    @Output() onSaveScores = new EventEmitter<boolean>();
    canSave: Boolean;
    selectedRoundIndex: number = 0;
    selectedRound: RoundViewModel;
    

    constructor() {
    }

    ngOnInit() {
        this.selectedRound = this.rounds[this.selectedRoundIndex];
    }

    onPreviousRound() {
        this.selectedRoundIndex--;
        this.selectedRound = this.rounds[this.selectedRoundIndex];
    }

    onNextRound() {
        this.selectedRoundIndex++;
        this.selectedRound = this.rounds[this.selectedRoundIndex];
    }

    finalizeMatch() {
        this.onFinalizeMatch.emit(true);
    }

    outputSaveScores() {
        this.onSaveScores.emit(true);
    }
}

class RoundViewModel {
    playerMatches: PlayerMatchViewModel[];
}