import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';

import { SessionsService } from '../sessions.service';

import { Session } from '../../model/session';
import { Team } from '../../model/team';
import { Player } from '../../model/player';

import { Observable } from 'RxJs';

@Injectable()
export class TeamsService {
    private headers: Headers;

    constructor(private sessionsService: SessionsService, private http: Http) {
        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
    }

    getTeamsFromSession(sessionId: number) {
        let session = this.sessionsService.getSession(sessionId);

        if (session == null) {
            return [];
        } else if (session.teams == null) {
            session.teams = [];
        }
        return session.teams;
    }

    addTeamToSession(team: Team, sessionId: number): Observable<Team> {
        return this.http.post('/api/team/' + sessionId,
            JSON.stringify({
                name: team.name,
                sessionId: sessionId,
                bye: team.bye,
                players: team.players
            }),
            { headers: this.headers })
            .map(res => res.json());
    }

    updateTeam(team: Team) {
        this.http.put('/api/team/' + team.teamId,
            JSON.stringify({
                teamId: team.teamId,
                name: team.name,
                sessionId: team.sessionId,
                bye: team.bye,
                players: team.players
            }),
            { headers: this.headers })
            .subscribe(
                );
    }

    addPlayerToTeam(player: Player, teamId: number): Observable<Player> {
        return this.http.post('/api/teamroster/' + teamId,
            JSON.stringify({
                playerId: player.playerId,
                handicap: player.handicap,
                name: player.name
            }),
            { headers: this.headers })
            .map(res => res.json());
    }

}
