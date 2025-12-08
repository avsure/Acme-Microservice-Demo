import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserProfileService, UserProfile } from '../../services/user-profile.service';

@Component({
  selector: 'app-user-profiles',
  standalone: true,
  imports: [CommonModule],
  template: `
    <h2>User Profiles</h2>

    <!-- Loading indicator -->
    <div *ngIf="loading">Loading users...</div>

    <!-- Users list -->
    <ul *ngIf="!loading && users?.length; else noUsers">
      <li *ngFor="let u of users; trackBy: trackById">
        {{ u.name }} {{ u.email }} ({{ u.age}})
      </li>
    </ul>

    <ng-template #noUsers>
      <div *ngIf="!loading">No users found.</div>
    </ng-template>
  `
})
export class UserProfilesComponent implements OnInit {
  users: UserProfile[] = [];
  loading = true;

  constructor(private svc: UserProfileService) {}

  ngOnInit() {
    this.svc.getAll().subscribe({
      next: (u) => {
        console.log('Users received:', u);
        this.users = u;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading users', err);
        this.loading = false;
      }
    });
  }

  trackById(index: number, item: UserProfile) {
    return item?.id;
  }
}
