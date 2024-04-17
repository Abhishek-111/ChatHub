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

  /**
   * Creates an instance of auth service.
   * @param configService 
   * @param http 
   */
 constructor(private configService:ConfigService,private http:HttpClient) {}

 /**
  * Login in the application
  * @param payload 
  * @returns login
  */
 public login(payload:IUserInput){
    return this.http.post(this.configService.getApiUrl()+'/api/user/register-user',payload);
 }
}
