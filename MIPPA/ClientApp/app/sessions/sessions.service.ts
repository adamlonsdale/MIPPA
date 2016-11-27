import { Injectable, EventEmitter } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';

import { Session } from '../model/session';

// Temporary
import { Team } from '../model/team';

import 'rxjs/Rx';

@Injectable()
export class SessionsService {

    private headers: Headers;

    databasePath = '/api/session';

    sessionsChanged = new EventEmitter<Session[]>();

    private sessions: Session[] = [];

    constructor(private http: Http) {
        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
    }

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
        return this.http.post(
            '/api/session/' + session.managerId,
            JSON.stringify({
                managerId: session.managerId,
                name: session.name,
                format: session.format,
                matchuptype: session.matchupType
            }),
            { headers: this.headers })
            .map(res => res.json())
            .subscribe(
            data => {
                session.sessionId = data.sessionId
                this.sessions.push(session);
                this.sessionsChanged.emit(this.sessions);
            }
                );
    }

    editSession(newSession: Session) {
        return this.http.put(
            '/api/session/' + newSession.sessionId,
            JSON.stringify({
                sessionId: newSession.sessionId,
                managerId: newSession.managerId,
                name: newSession.name,
                format: newSession.format,
                matchuptype: newSession.matchupType
            }),
            { headers: this.headers })
            .subscribe(
            data => {
            }
            );
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
