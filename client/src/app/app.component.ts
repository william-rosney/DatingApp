import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DatingApp';
  users: any;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser(){
    const user: User = JSON.parse(localStorage.getItem('user')); //Get les infos du current logged in user à partir du stockage local du navigateur
    this.accountService.setCurrentUser(user);
  }

  
}
