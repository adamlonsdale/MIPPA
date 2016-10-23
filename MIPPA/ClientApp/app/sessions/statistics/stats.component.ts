import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Session } from '../../model/session';
import { StatsService } from './stats.service';

import { Subscription } from 'rxjs/Rx';

@Component({
    selector: 'cp-stats',
    template: require('./stats.component.html')
})
export class StatsComponent implements OnInit {
    private subscription: Subscription;
    eightBallStatistics: StatisticsViewModel;
    nineBallStatistics: StatisticsViewModel;
    sessionId: number;
    session: Session;


    constructor(private router: Router,
        private activatedRoute: ActivatedRoute,
        private statsService: StatsService) { }

    ngOnInit() {
        this.subscription =
            this.activatedRoute.params.subscribe(
                (params: any) => {
                    this.sessionId = +params['sessionId']
                    console.log(this.sessionId);

                    this.statsService.getSession(this.sessionId).subscribe(
                        data => {
                            this.session = data;
                            console.log(this.session);
                            this.loadStatsForSession();
                        });
                }
            );


    }

    loadStatsForSession() {
        if (this.session.format == 0) {
            // 8 ball only
            this.statsService.GetStatistics(this.session.sessionId, this.session.format)
                .subscribe(
                data => this.eightBallStatistics = data,
                e => console.log(e.message),
                () => {
                });
        }
        else if (this.session.format == 1) {
            // 9 ball only
            this.statsService.GetStatistics(this.session.sessionId, this.session.format)
                .subscribe(
                data => this.nineBallStatistics = data,
                e => console.log(e.message),
                () => {
                    console.log(this.eightBallStatistics);
                    console.log(this.nineBallStatistics);
                });
        }
        else if (this.session.format == 2 || this.session.format == 3) {
            // BOTH
            this.statsService.GetStatistics(this.session.sessionId, 0)
                .subscribe(
                data => this.eightBallStatistics = data,
                e => console.log(e.message),
                () => {
                    console.log(this.eightBallStatistics);
                    console.log(this.nineBallStatistics);
                });
            this.statsService.GetStatistics(this.session.sessionId, 1)
                .subscribe(
                data => this.nineBallStatistics = data,
                e => console.log(e.message),
                () => {
                    console.log(this.eightBallStatistics);
                    console.log(this.nineBallStatistics);
                });
        }

        console.log(this.session.format);
        
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}

class StatisticsViewModel {
    teamStatistics: TeamStatisticsViewModel[];
    playerStatistics: PlayerStatisticsViewModel[];
}

class TeamStatisticsViewModel {
    name: string;
    totalWins: number;
}

class PlayerStatisticsViewModel {
    name: string;
    totalWins: number;
}