﻿import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { HandicapsService } from './handicaps.service';
import { SessionsService } from '../sessions.service';

import { Session } from '../../model/session';

import { Subscription } from 'rxjs/Rx';

@Component({
    selector: 'cp-handicaps',
    template: require('./handicaps.component.html')
})
export class HandicapsComponent implements OnInit, OnDestroy {
    private subscription: Subscription;
    sessionId: number;
    session: Session;

    viewModels: any = [];

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

                        this.handicapsService.getHandicapsForSession(this.sessionId)
                            .subscribe(
                            data => this.viewModels = data);
                    }
            );
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

}
