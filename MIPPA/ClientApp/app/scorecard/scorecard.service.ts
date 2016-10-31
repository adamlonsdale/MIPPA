import { Injectable } from '@angular/core'
import { Http, Response, Headers, URLSearchParams } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/share';
import 'rxjs/add/operator/timeout';
import 'rxjs/add/operator/retryWhen';

@Injectable()
export class ScorecardService {
    private headers: Headers;

    constructor(public http: Http) {
        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
    }

    AddPlayer(player: any): Observable<any> {
        return this.http.post(
            '/api/player',
            JSON.stringify({
                name: player.name
            }),
            { headers: this.headers })
            .map(res => res.json());
    }

    AddPlayerToTeam(player: any, teamId: number): Observable<any> {
        return this.http.post(
            '/api/teamroster/' + teamId,
            JSON.stringify(player),
            { headers: this.headers })
            .map(res => res.json());
    }

    GetPlayersQuery(term: string, sessionId: number): Observable<any[]> {
        let params: URLSearchParams = new URLSearchParams();
        params.set('term', term);


        return this.http.get('/api/player/' + sessionId, { search: params })
            .map(res => res.json());
    }

    GetStatistics(sessionId: number, format: number): Observable<any> {
        return this.http.get('api/statistics/' + sessionId + '/' + format).map(res => res.json());
    }

    FinalizeMatch(scorecardId: number): Observable<any> {
        return this.http.post('/api/teamresult/' + scorecardId,
            {},
            { headers: this.headers })
            .map(res => res.json());
    }

    ResetScorecard(scorecardId: number) {
        return this.http.get('/api/resetrequest/' + scorecardId).map(res => res.json());
    }

    GetResetRequests(sessionId: number) {
        return this.http.get('/api/resetrequest/' + sessionId).map(res => res.json());
    }

    RequestReset(scorecardId: number, resetRequest: any) {
        return this.http.put('api/resetrequest/' + scorecardId, JSON.stringify(resetRequest), { headers: this.headers })
            .map(res => res.json());
    }

    GetPlayerResults(scorecardId: number): Observable<any> {
        return this.http.get('/api/playerresult/' + scorecardId)
            .map(res => res.json());
    }

    GetTeamResults(scorecardId: number): Observable<any> {
        return this.http.get('/api/teamresult/' + scorecardId)
            .map(res => res.json());
    }

    UpdatePlayerScores(playerMatchViewModel: any): Observable<any> {
        return this.http.put('api/playermatch',
            playerMatchViewModel,
            { headers: this.headers })
            .retryWhen(error => error.delay(500))
            .timeout(60000, new Error('timeout exceeded'))
            .map(res => res.json());
    }

    GetScorecardInformation(scorecardId: number): Observable<any> {
        console.log('MIPPAService obtaining scorecard information');
        return this.http.get('api/scorecard/' + scorecardId).map(res => res.json()).share();
    }

    PostScorecardLineup(scorecardInfo: any): Observable<any> {
        return this.http.post(
            'api/Scorecard',
            JSON.stringify({
                scorecardId: scorecardInfo.scorecardId,
                homePlayers: scorecardInfo.homePlayers,
                awayPlayers: scorecardInfo.awayPlayers,
                state: scorecardInfo.state,
                format: scorecardInfo.format,
                matchupType: scorecardInfo.matchupType,
                numberOfTables: scorecardInfo.numberOfTables
            }),
            { headers: this.headers })
            .map(res => res.json());
    }
}