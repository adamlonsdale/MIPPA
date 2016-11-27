import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Team } from '../../model/team';
import { Player } from '../../model/player';

import { SessionsService } from '../sessions.service';
import { TeamsService } from './teams.service';

import { Subscription } from 'RxJs';

@Component({
    selector: 'cp-team',
    template: require('./team.component.html'),
    styles: [require('./team.component.css')]
})
export class TeamComponent implements OnInit {
    @Input() team: Team;
    @Input() collapsed: boolean;
    editName: boolean;
    playerAdd: boolean;
    sessionId: number;

    private subscription: Subscription;

    newPlayer: Player;
    selectedPlayer: Player;

    constructor(private sessionsService: SessionsService, private teamsService: TeamsService, private activatedRoute: ActivatedRoute) {
        this.newPlayer = new Player(0, '', 0);
    }

    ngOnInit() {
        this.subscription =
            this.activatedRoute.parent.params.subscribe(
                (params: any) => {
                    this.sessionId = +params['sessionId'];
                }
            );
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }

    onEditName() {
        this.editName = true;
    }

    onSaveName(value: any) {
        this.team.name = value.name;

        this.sessionsService.storeSessions()
            .subscribe(
            (res: any) => {

            }
            );

        this.teamsService.updateTeam(this.team);
        this.editName = false;
    }

    onCancel() {
        this.editName = false;
    }

    onPlayerAdd() {
        this.playerAdd = true;
    }

    onCancelPlayer() {
        this.playerAdd = false;
    }

    addPlayerToTeam(player: any) {
        this.team.players.push(player);
    }

    onOrderUp() {

    }

    onOrderDown() {

    }

}
