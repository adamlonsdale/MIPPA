import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'RxJs/rx';

import { SessionsService } from '../sessions.service';

@Injectable()
export class ScheduleService {
  

  constructor(private sessionsService: SessionsService, private http: Http) { }

  getScheduleFromSession(sessionId: number) {
    let session = this.sessionsService.getSession(sessionId);

    if (session == null) {
      return null;
    } else if (session.schedules == null) {
      return null
    }
    return session.schedules;
  }

  GetSchedulesForSession(sessionId: number): Observable<any[]> {
      return this.http.get('/api/schedule/' + sessionId)
          .map(res => res.json());
  }

}
