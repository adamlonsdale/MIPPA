import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Subscription } from 'rxjs/Rx';

import { Team } from '../../model/team';
import { TeamsService } from './teams.service';
import { SessionsService } from '../sessions.service';

@Component({
  selector: 'cp-teams',
  template: require('./teams.component.html')
})
export class TeamsComponent implements OnInit, OnDestroy {
  private subscription: Subscription;
  private sessionId: number;
  setOrder: boolean;

  teams: Team[] = [];

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private teamsService: TeamsService,
    private sessionsService: SessionsService) {

    this.sessionsService.sessionsChanged.subscribe(
      (sessions: any) => this.loadTeams()
    );
  }

  ngOnInit() {
    this.subscription =
      this.activatedRoute.parent.params.subscribe(
        (params: any) => {
          this.sessionId = +params['sessionId']
          this.loadTeams()
        }
      );
  }

  onAddTeam() {
    this.teams.push(new Team('Team ' + (this.teams.length + 1), [], false));
  }

  onAddBye() {
    this.teams.push(new Team('BYE', [], true));
  }

  onSetOrder() {
    this.setOrder = !this.setOrder;
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  loadTeams() {
    this.teams = this.teamsService.getTeamsFromSession(this.sessionId);
  }

}