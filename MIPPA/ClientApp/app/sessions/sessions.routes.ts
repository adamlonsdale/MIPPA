import { Routes } from "@angular/router";

import { ManagerComponent } from './manager.component';
import { TeamsComponent } from './teams/teams.component';

import { MANAGER_ROUTES } from './manager.routes';

export const SESSION_ROUTES: Routes = [
    { path: ':sessionId/manage', component: ManagerComponent, children: MANAGER_ROUTES },
    { path: '**', redirectTo: '' }
];