import {Component, Input, Output, EventEmitter, OnChanges, SimpleChanges} from '@angular/core';
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
    @Input() filterPlayers: Player[] = [];
    @Output() onAdd = new EventEmitter<Player>();
    public dataSource: Observable<any>;
    public asyncSelected: string = '';
    players: Player[];
    public typeaheadLoading: boolean = false;
    public typeaheadNoResults: boolean = false;
    public selectedPlayer: any;
    public playerExists: boolean = false;
    disableHandicap: boolean = true;
    selectedHandicap: number = 4;
    pendingAdd: boolean = false;

    public constructor(private scorecardService: ScorecardService) {
        this.dataSource = Observable.create((observer: any) => {
            this.dataSource = Observable.create((observer: any) => {
                observer.next(this.players.filter((player: any) => {
                    return true;
                }));
            });
        });
    }

    ngOnChanges(changes: SimpleChanges) {
        console.log(changes);
    }

    onAddAsSub() {
        this.pendingAdd = true;

        if (this.checkIfDuplicatePlayer(this.selectedPlayer)) {
            this.pendingAdd = false;
            return;
        }

        if (!this.selectedPlayer.existsInSession) {
            this.selectedPlayer.handicap = this.selectedHandicap;
        }
        this.scorecardService.AddPlayerToTeam(this.selectedPlayer, this.teamId)
            .subscribe(
            data => {
                this.onAdd.emit(this.selectedPlayer);
                this.pendingAdd = false;
                this.clear();
            });
    }

    onAddNew() {
        this.pendingAdd = true;

        if (this.checkIfDuplicatePlayer(this.selectedPlayer)) {
            this.pendingAdd = false;
            return;
        }

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
                            this.pendingAdd = false;
                            this.clear();
                        });
                }
            });
    }

    checkIfDuplicatePlayer(player: Player): boolean {
        if (this.filterPlayers.length == 0) {
            return false;
        }

        var playerExists =
            this.filterPlayers.filter(p => p.playerId == player.playerId).length > 0;

        return playerExists;
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