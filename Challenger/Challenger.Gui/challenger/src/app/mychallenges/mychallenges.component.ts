import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { NewChallenge } from '../shared/models/NewChallenge';
import { ChallengeOverview } from '../shared/models/ChallengeOverview';

@Component({
  selector: 'app-mychallenges',
  templateUrl: './mychallenges.component.html',
  styleUrls: ['./mychallenges.component.css']
})
export class MychallengesComponent implements OnInit {

  myChallenges: Array<ChallengeOverview>;

  constructor(private api: ApiService) { }

  ngOnInit() {
    this.api.getMyChallenges().subscribe(data => {
      this.myChallenges = data.Data;
    })
  }
}
