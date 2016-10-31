import {Component, Input, Output, EventEmitter} from '@angular/core';
import {Observable} from 'rxjs/Observable';
import {Player} from '../model/player';
import {ScorecardService} from './scorecard.service';

declare var jQuery: any;

@Component({
    selector: 'add-substitute',
    template: require('./substitute.component.html')
})
export class SubstituteComponent {
    @Input() sessionId: number;
    @Input() teamId: number;
    @Output() onAdd = new EventEmitter<Player>();
    public dataSource: Observable<any>;
    public asyncSelected: string = '';
    players: any[];
    public typeaheadLoading: boolean = false;
    public typeaheadNoResults: boolean = false;
    public selectedPlayer: any;
    public playerExists: boolean = false;
    disableHandicap: boolean = true;
    selectedHandicap: number = 4;

    public constructor(private scorecardService: ScorecardService) {
        this.dataSource = Observable.create((observer: any) => {
            this.dataSource = Observable.create((observer: any) => {
                observer.next(this.players.filter((player: any) => {
                    return true;
                }));
            });
        });
    }

    onAddAsSub() {
        if (!this.selectedPlayer.existsInSession) {
            this.selectedPlayer.handicap = this.selectedHandicap;
        }
        this.scorecardService.AddPlayerToTeam(this.selectedPlayer, this.teamId)
            .subscribe(
            data => {
                this.onAdd.emit(this.selectedPlayer);
            });
    }

    onAddNew() {
        var player: any = {};
        player.name = this.asyncSelected;

        this.scorecardService.AddPlayer(player)
            .subscribe(
            data => {
                player = data;
                player.handicap = this.selectedHandicap;

                if (player.hasOwnProperty('playerId')) {
                    this.scorecardService.AddPlayerToTeam(player, this.teamId)
                        .subscribe(
                        data => {
                            this.onAdd.emit(player);
                        });
                }
            });
    }

    clear() {
        this.asyncSelected = '';
        this.selectedHandicap = 4;
        this.selectedPlayer = null;
        this.disableHandicap = true;
        this.typeaheadNoResults = false;
    }

    getPlayers() {
        this.scorecardService.GetPlayersQuery(this.asyncSelected, this.sessionId)
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
        this.selectedHandicap = 4;

        this.disableHandicap = false;
    }

    public typeaheadOnSelect(e: any): void {
        this.selectedPlayer = e.item;
        this.selectedHandicap = this.selectedPlayer.handicap;

        if (this.selectedPlayer != null && !this.selectedPlayer.existsInSession) {
            this.disableHandicap = false;
        } else {
            this.disableHandicap = true;
        }

        this.playerExists = true;
    }
}