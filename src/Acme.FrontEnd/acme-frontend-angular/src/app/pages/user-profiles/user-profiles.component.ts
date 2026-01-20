import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserProfileService, UserProfile } from '../../services/user-profile.service';

@Component({
  selector: 'app-user-profiles',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-profiles.component.html'
})
export class UserProfilesComponent implements OnInit {

  users: UserProfile[] = [];
  loading = true;

  newUser: Partial<UserProfile> = {
    name: '',
    email: '',
    age: 0
  };

  constructor(private svc: UserProfileService) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.svc.getAll().subscribe({
      next: (u) => {
        this.users = u;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading users', err);
        this.loading = false;
      }
    });
  }

  loadUser(id: string) {
    this.svc.getById(id).subscribe(u => {
      alert(`User: ${u.name}\nEmail: ${u.email}\nAge: ${u.age}`);
    });
  }

  createUser() {
    this.svc.create(this.newUser).subscribe(() => {
      this.newUser = { name: '', email: '', age: 0 };
      this.loadUsers();
    });
  }

  trackById(index: number, item: UserProfile) {
    return item?.id;
  }
}
