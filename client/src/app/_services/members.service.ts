import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl: string = environment.apiUrl;
  members: Member[] = [];

  constructor(private http: HttpClient) { }

  getMembers() {
    console.log(this.members);
    //On check si members contient des membres. Si oui on retourne members as Observable avec of()
    if (this.members.length > 0) return of(this.members);
    //Sinon on fait la requête HTTP et on mets à jour members
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members => {
        this.members = members;
        return members;
      })
    );
  }

  getMember(username) {
    //On check si member contient déjà le member recherché avec username
    const member = this.members.find(x => x.username === username);
    if (member !== undefined) return of(member);
    //Sinon on effectue la requete http 
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    //Après la put request on met à jour notre le member dans notre members array
    return this.http.put<Member>(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }

} 
