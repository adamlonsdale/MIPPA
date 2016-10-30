import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Subscription } from 'rxjs/Rx';

import { ScheduleService } from './schedule.service';
import { SessionsService } from '../sessions.service';

import { AppComponent } from '../../app.component';

import { ScheduleViewModel } from '../../viewmodel/schedule/schedule-view-model';

@Component({
    selector: 'cp-schedule',
    template: require('./schedule.component.html')
})
export class ScheduleComponent implements OnInit {
    private subscription: Subscription;
    private sessionId: number;

    mode: string;

    schedules: any[];

    constructor(
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private scheduleService: ScheduleService,
        private sessionsService: SessionsService) {

        this.sessionsService.sessionsChanged.subscribe(
            (sessions: any) => this.loadSchedule()
        );
    }

    ngOnInit() {

        if (this.activatedRoute.parent.parent === null) {
            this.mode = "view";
            this.subscription =
                this.activatedRoute.params.subscribe(
                    (params: any) => {
                        this.sessionId = +params['sessionId'];

                        this.scheduleService.GetSchedulesForSession(this.sessionId)
                            .subscribe(data => this.schedules = data);
                    }
                );
        }
        else {
            this.mode = "edit";
            this.subscription =
                this.activatedRoute.parent.params.subscribe(
                    (params: any) => {
                        this.sessionId = +params['sessionId']
                        this.loadSchedule();
                    }
                );
        }
    }

    loadSchedule() {
        this.schedules = this.scheduleService.getScheduleFromSession(this.sessionId);
    }

    openScorecard(scorecardId: number) {
        this.router.navigate(['/', 'app', 'scorecard', scorecardId]);
    }

}
