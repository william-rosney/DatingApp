import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from '../_models/member';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  user: User;
  member: Member;
  @ViewChild('editForm')
  editForm: NgForm;

  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }
  constructor(private accountService: AccountService, private membersService: MembersService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user,
    })
  }


  ngOnInit(): void {
    this.loadMember();
  }
  loadMember() {
    this.membersService.getMember(this.user.username).subscribe({
      next: member => this.member = member,
    })
  }

  updateMember() {
    this.membersService.updateMember(this.member).subscribe({
      next: () => {
        this.toastr.success("Profile changed successfully!");
        this.editForm.reset(this.member);
      }
    });
  }

}
