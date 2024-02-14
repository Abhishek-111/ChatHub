import { Injectable } from '@angular/core';
import { ConfigService } from '../config/config.service';
import { HttpClient } from '@angular/common/http';
import { IUserInput } from '../../Interfaces/user/IUserInput';
import { Observable } from 'rxjs';
import { IUserOutput } from '../../Interfaces/user/IUserOutput';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
 constructor(private configService:ConfigService,private http:HttpClient) {}

 public login(payload:IUserInput){
    return this.http.post(this.configService.getApiUrl()+'/api/user/register-user',payload);
 }
}
