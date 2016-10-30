import { Injectable, EventEmitter } from '@angular/core';
import { Http, Headers, Response, URLSearchParams } from '@angular/http';

import { Player } from '../model/player';

import { Observable }     from 'rxjs/Observable';

import 'rxjs/Rx';

@Injectable()
export class TestingService {

    databasePath = '/api/session';

    constructor(private http: Http) { }

    GetPlayersQuery(term: string): Observable<Player[]> {
        let params: URLSearchParams = new URLSearchParams();
        params.set('term', term);


        return this.http.get('/api/player', { search: params })
            .map(res => res.json());
    }

}
