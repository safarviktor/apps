import { ChallengeType } from './ChallengeType';

export class ChallengeOverview
{
    Id: Number;
    Name: string;
    CurrentTotal: number;
    LastEntry: Date;
    LasEntryCount: number;
    TodayCount: number;
    Type: ChallengeType;
}