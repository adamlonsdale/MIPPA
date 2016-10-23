import { Component, Input, OnInit } from '@angular/core';

import { Team } from '../../model/team';
import { Player } from '../../model/player';

import { SessionsService } from '../sessions.service';

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

  newPlayer: Player;
  selectedPlayer: Player;

  constructor(private sessionsService: SessionsService) {
    this.newPlayer = new Player(0, '', 0);
   }

  ngOnInit() {
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

  onSubmitPlayer(player: Player) {

    if (this.team.players == undefined) {
      this.team.players = [];
    }

    this.team.players.push(player);
    this.playerAdd = false;
  }

  onOrderUp() {

  }

  onOrderDown() {

  }

}
