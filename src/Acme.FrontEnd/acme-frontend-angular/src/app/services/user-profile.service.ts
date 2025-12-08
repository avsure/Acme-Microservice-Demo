import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { USER_API_BASE_URL } from '../app.config';

export interface UserProfile {
  id: string;
  name: string;
  email: string;
  age: number;
}

@Injectable({
  providedIn: 'root'
})

export class UserProfileService {
  
 private readonly userprofileUrl: string;

  constructor
  (
    private http: HttpClient,
    @Inject(USER_API_BASE_URL) private userApiUrl: string 
  ) 
  {
    this.userprofileUrl = `${userApiUrl}/users/api/userprofiles`;
  }

  getAll(): Observable<UserProfile[]> {
    return this.http.get<UserProfile[]>(this.userprofileUrl);
  }

  getById(id: string): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.userprofileUrl}/${id}`);
  }

  create(profile: Partial<UserProfile>): Observable<any> {
    return this.http.post(this.userprofileUrl, profile);
  }
}
