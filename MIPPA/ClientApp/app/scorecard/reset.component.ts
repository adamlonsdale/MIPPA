import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ScorecardService } from './scorecard.service';
import { SessionsService } from '../sessions/sessions.service';

import { Subscription } from 'RxJs/rx';

@Component({
    selector: 'reset-component',
    template: require('./reset.component.html')
})
export class ResetComponent implements OnInit, OnDestroy {
    private subscription: Subscription;
    sessionId: number;
    managerId: number;
    resetRequests: any[];

    constructor(private activatedRoute: ActivatedRoute, private router: Router, private scorecardService: ScorecardService, private sessionsService: SessionsService) {
    }

    ngOnInit() {
        this.subscription =
            this.activatedRoute.parent.params.subscribe(
            param => {
                this.sessionId = +param['sessionId'];

                this.activatedRoute.parent.parent.params.subscribe(
                    param => {
                        this.managerId = +param['managerId'];
                    });

                this.scorecardService.GetResetRequests(this.sessionId).
                    subscribe(data => this.resetRequests = data);
            });
    }

    onResetClick(scorecardId: number) {
        this.scorecardService.ResetScorecard(this.sessionId, scorecardId).subscribe(data => this.resetRequests = data);
        this.sessionsService.fetchSessions(this.managerId);
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}