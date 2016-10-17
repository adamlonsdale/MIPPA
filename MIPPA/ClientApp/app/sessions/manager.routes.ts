import { Routes } from "@angular/router";

import { TeamsComponent } from './teams/teams.component';
import { ScheduleComponent } from './schedule/schedule.component';

export const MANAGER_ROUTES: Routes = [
    { path: 'teams', component: TeamsComponent },
    { path: 'schedule', component: ScheduleComponent }
];