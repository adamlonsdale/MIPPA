import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Subscription } from 'RxJs';

import { SchedulerService } from './scheduler.service';
import { WeekViewModel } from '../model/weekviewmodel';
import { TeamViewModel } from '../model/teamviewmodel';
import { MatchViewModel } from '../model/matchviewmodel';

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

    previousWeekExists: boolean;
    nextWeekExists: boolean;

    editingLocation: boolean;
    editingTable: boolean;
    editingDate: boolean;

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

    addToMatchup() {
        if (this.selectedAwayTeam != null && this.selectedHomeTeam != null) {
            this.viewModel.matchViewModels.push(new MatchViewModel(this.selectedHomeTeam, this.selectedAwayTeam, "", ""));

            this.selectedAwayTeam.scheduled = true;
            this.selectedHomeTeam.scheduled = true;

            this.selectedAwayTeam = null;
            this.selectedHomeTeam = null;
        }
    }

    startOver() {
        this.viewModel.matchViewModels = new Array<MatchViewModel>();

        for (let team of this.viewModel.teamViewModels) {
            team.scheduled = false;
        }
    }

    saveMatchups() {
        this.schedulerService.PostMatchups(this.sessionId, this.scheduleIndex, this.viewModel.matchViewModels)
            .subscribe(data => { });
    }

    selectTeam(team: TeamViewModel) {
        if (this.selectedAwayTeam == null) {
            this.selectedAwayTeam = team;
        }
        else if (this.selectedHomeTeam == null) {
            this.selectedHomeTeam = team;
        }
        else {
            this.selectedAwayTeam = team;
            this.selectedHomeTeam = null;
        }

    }
}
