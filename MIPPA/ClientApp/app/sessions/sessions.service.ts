import { Injectable, EventEmitter } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';

import { Session } from '../model/session';

// Temporary
import { Team } from '../model/team';

import 'rxjs/Rx';

@Injectable()
export class SessionsService {

    databasePath = '/api/session';

    sessionsChanged = new EventEmitter<Session[]>();

    private sessions: Session[] = [];

    constructor(private http: Http) { }

    getSessions() {
        return this.sessions;
    }

    getSession(sessionId: number) {
        if (this.sessions.length >= 0) {
            return this.sessions.filter(session => session.sessionId == sessionId)[0];
        }
        else {

        }
        return null;
    }

    addSession(session: Session) {
        this.sessions.push(session);
        this.sessionsChanged.emit(this.sessions);
    }

    editSession(oldSession: Session, newSession: Session) {
        this.sessions[this.sessions.indexOf(oldSession)] = newSession;
    }

    storeSessions() {
        const body = JSON.stringify(this.sessions);
        const headers = new Headers(
            {
                'Content-Type': 'application/json'
            }
        );

        return this.http.put(this.databasePath, body, { headers: headers });
    }

    fetchSessions(managerId: number) {
        console.log(managerId);
        return this.http.get(this.databasePath + '/' + managerId)
            .retryWhen(error => error.delay(500))
            .timeout(2000, new Error('delay exceeded'))
            .map((response: Response) => response.json())
            .subscribe(
            (data: Session[]) => {
                if (data) {
                    this.sessions = data;
                    this.sessionsChanged.emit(this.sessions);
                }
            }
            );
    }

    getCountries() {
        return this.http.get('https://concept-d15cf.firebaseio.com/sessions.json')
            .toPromise()
            .then(res => <any[]>res.json())
            .then(data => { return data; });
    }

    updateAllHandicapsForPlayerOnTeam(sessionId: number, playerId: number, handicap: number) {
        var session = this.getSession(sessionId);

        if (session != null) {
            for (let team of session.teams) {
                for (let player of team.players) {
                    if (player.playerId == playerId) {
                        player.handicap = handicap;
                    }
                }
            }
        }

        this.sessionsChanged.emit(this.sessions);
    }

}
