import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ChallengeOverview } from './shared/models/ChallengeOverview';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private client: HttpClient) { }

  getMyChallenges() 
  {    
    return this.client.get<JsonResult>('http://localhost:59797/api/challenges/list');
  }

  getChallenge(challengeId: number)
  {
    return this.client.get<JsonResult>(`http://localhost:59797/api/challenges/details/${challengeId}`);
  }

  addChallenge(name: string, type: string)
  {
    return this.client.post('http://localhost:59797/api/challenges/create', { name, type });
  }
}
