import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UniversalModule } from 'angular2-universal';
// Routing import
import { ConceptsRoutingModule } from './app-routing.module';
// Services
import { SessionsService } from './sessions/sessions.service';
import { TeamsService } from './sessions/teams/teams.service';
import { ScheduleService } from './sessions/schedule/schedule.service';
import { LoginService } from './login/login.service';
import { ScorecardService } from './scorecard/scorecard.service';
import { HandicapsService } from './sessions/handicaps/handicaps.service';
import { StatsService } from './sessions/statistics/stats.service';

import { AppComponent } from './app.component';
import { SessionsComponent } from './sessions/sessions.component';
import { HandicapsComponent } from './sessions/handicaps/handicaps.component';
import { TeamsComponent } from './sessions/teams/teams.component';
import { SessionListComponent } from './sessions/session-list/session-list.component';
import { SessionItemComponent } from './sessions/session-list/session-item.component';
import { SessionEditComponent } from './sessions/session-edit/session-edit.component';
import { ManagerComponent } from './sessions/manager.component';
import { ScheduleComponent } from './sessions/schedule/schedule.component';
import { TeamComponent } from './sessions/teams/team.component';
import { PlayerEditComponent } from './sessions/teams/player-edit/player-edit.component';
import { LoginComponent } from './login/login.component';
import { PlayerMatchComponent } from './scorecard/player-match.component';
import { HandicapEditComponent } from './sessions/handicaps/handicap-edit.component';
import { StatsComponent } from './sessions/statistics/stats.component';
import { ResetComponent } from './scorecard/reset.component';

// Autocomplete
import { AutoCompleteModule } from 'primeng/primeng';
import { ScorecardComponent } from './scorecard/scorecard.component';

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        SessionsComponent,
        TeamsComponent,
        SessionListComponent,
        SessionItemComponent,
        SessionEditComponent,
        ManagerComponent,
        ScheduleComponent,
        TeamComponent,
        PlayerEditComponent,
        ScorecardComponent,
        PlayerMatchComponent,
        HandicapsComponent,
        HandicapEditComponent,
        StatsComponent,
        ResetComponent
    ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        ConceptsRoutingModule,
        FormsModule,
        AutoCompleteModule
    ],
    providers: [SessionsService, TeamsService, ScheduleService, LoginService, ScorecardService, HandicapsService, StatsService],
    bootstrap: [AppComponent]
})
export class AppModule { }
