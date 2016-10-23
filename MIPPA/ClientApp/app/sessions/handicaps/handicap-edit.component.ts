import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { HandicapsService } from './handicaps.service';
import { SessionsService } from '../sessions.service';

import { Session } from '../../model/session';

import { Subscription } from 'rxjs/Rx';

@Component({
    selector: '[handicapTr]',
    template: require('./handicap-edit.component.html')
})
export class HandicapEditComponent implements OnInit {
    private subscription: Subscription;
    sessionId: number;
    session: Session;
    editing: boolean;

    @Input('handicapTr') viewModel: any;

    constructor(
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private handicapsService: HandicapsService,
        private sessionsService: SessionsService) {
    }

    ngOnInit() {
            this.subscription =
                this.activatedRoute.parent.params.subscribe(
                    (params: any) => {
                        this.sessionId = +params['sessionId']
                        this.session = this.sessionsService.getSession(this.sessionId);
                    }
                );
    }

    onEditClick(playerId: number) {
        if (this.editing) {
            this.handicapsService.updateHandicapForPlayer(this.sessionId, playerId, this.viewModel.handicap).subscribe();
            this.sessionsService.updateAllHandicapsForPlayerOnTeam(this.sessionId, playerId, this.viewModel.handicap);

        }

        this.editing = !this.editing;
    }

}
