import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'RxJs';

import { SchedulerService } from './scheduler.service';
import { WeekViewModel } from '../model/weekviewmodel';
import { TeamViewModel } from '../model/teamviewmodel';

@Component({
    selector: 'scheduler',
    template: require('./scheduler.component.html'),
    styles: [require('./scheduler.component.css')]
})
export class SchedulerComponent implements OnInit, OnDestroy {
    private subscription: Subscription;
    sessionId: number;
    scheduleIndex: number;
    viewModel: WeekViewModel;

    selectedHomeTeam: TeamViewModel;
    selectedAwayTeam: TeamViewModel;

    constructor(private activatedRoute: ActivatedRoute, private schedulerService: SchedulerService) {
    }

    ngOnInit() {
        this.subscription =
            this.activatedRoute.params.subscribe(
                (params: any) => {
                    this.sessionId = +params['sessionId'];
                    this.scheduleIndex = +params['scheduleIndex'];
                }
            );
        this.schedulerService.GetWeekViewModel(this.sessionId, this.scheduleIndex)
            .subscribe(data => this.viewModel = data);

    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    selectTeam(team: TeamViewModel) {
        if (this.selectedHomeTeam == null) {
            this.selectedHomeTeam = team;
        }
        else if (this.selectedAwayTeam == null) {
            this.selectedAwayTeam = team;
        }

    }
}
