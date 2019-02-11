import { Component, OnInit, OnDestroy } from '@angular/core';
import { ApiService } from '../api.service';
import { ChallengeDetail } from '../shared/models/ChallengeDetail';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-challenge',
  templateUrl: './challenge.component.html',
  styleUrls: ['./challenge.component.css']
})
export class ChallengeComponent implements OnInit, OnDestroy {

  thisChallenge : ChallengeDetail;
  id: number;
  private sub: any;

  constructor(private api: ApiService, private route: ActivatedRoute) { }

  ngOnInit() {    

    this.sub = this.route.params.subscribe(params => {
      this.id = +params['id']; // (+) converts string 'id' to a number

      this.api.getChallenge(this.id).subscribe(data => {
        this.thisChallenge = data.Data;
      })
   });    
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
  
  addChallenge()
  {
    this.api.addChallenge("", "");
  }

}
