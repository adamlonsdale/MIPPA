import {Component} from '@angular/core';
import {Observable} from 'rxjs/Observable';
import {Player} from '../model/player';
import {TestingService} from './testing.service';

@Component({
    selector: 'my-query-player',
    template: require('./testing.component.html')
})
export class TestingComponent {
    public dataSource: Observable<any>;
    public asyncSelected: string = '';
    players: Player[];
    public typeaheadLoading: boolean = false;
    public typeaheadNoResults: boolean = false;
    public selectedPlayer: Player;
    public playerExists: boolean = false;

    public constructor(private mippaService: TestingService) {
        this.dataSource = Observable.create((observer: any) => {
            this.dataSource = Observable.create((observer: any) => {
                observer.next(this.players.filter((player: any) => {
                    return true;
                }));
            });
        });
    }

    getPlayers() {
        this.mippaService.GetPlayersQuery(this.asyncSelected)
            .subscribe(
            players => this.players = players);
    }

    public changeTypeaheadLoading(e: boolean): void {
        this.typeaheadLoading = e;
        this.getPlayers();
    }

    public changeTypeaheadNoResults(e: boolean): void {
        this.typeaheadNoResults = e;
        this.playerExists = false;
        this.selectedPlayer = null;
    }

    public typeaheadOnSelect(e: any): void {
        this.selectedPlayer = e.item;
        this.playerExists = true;
    }
}