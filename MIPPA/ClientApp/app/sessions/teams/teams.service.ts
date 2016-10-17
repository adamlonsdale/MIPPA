import { Injectable } from '@angular/core';

import { SessionsService } from '../sessions.service';

import { Session } from '../../model/session';
import { Team } from '../../model/team';

@Injectable()
export class TeamsService {

  constructor(private sessionsService: SessionsService) { }

  getTeamsFromSession(sessionId: number) {
    let session = this.sessionsService.getSession(sessionId);

    if (session == null) {
      return [];
    } else if (session.teams == null) {
      session.teams = [];
    }
    return session.teams;
  }

}
