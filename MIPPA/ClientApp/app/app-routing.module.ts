import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SessionsComponent } from './sessions/sessions.component';
import { ScorecardComponent } from './scorecard/scorecard.component';
import { StatsComponent } from './sessions/statistics/stats.component';
import { LoginComponent } from './login/login.component';
import { ScheduleComponent } from './sessions/schedule/schedule.component';
import { SchedulerComponent } from './scheduler/scheduler.component';

import { SESSION_ROUTES } from './sessions/sessions.routes';

const routes: Routes = [
    { path: '', redirectTo: '/app/login', pathMatch: 'full' },
    { path: 'app/login', component: LoginComponent },
    { path: 'app/administration/:managerId/sessions', component: SessionsComponent, children: SESSION_ROUTES },
    { path: 'app/scorecard/:scorecardId', component: ScorecardComponent },
    { path: 'app/schedule/:sessionId', component: ScheduleComponent },
    { path: 'app/scheduler/:sessionId/:scheduleIndex', component: SchedulerComponent },
    { path: 'app/statistics/:sessionId', component: StatsComponent },
    { path: '**', redirectTo: '/app/login' },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: []
})
export class ConceptsRoutingModule { }
