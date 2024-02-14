import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IUserOutput } from 'src/app/core/Interfaces/user/IUserOutput';
import { ConfigService } from 'src/app/core/services/config/config.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private configService:ConfigService,private http:HttpClient) {}

  public getAllUsers():Observable<IUserOutput[]>{
    return this.http.get<IUserOutput[]>(this.configService.getApiUrl()+'/api/user/all-users');
  }

  // public getUserById(id:number):Observable<IUserOutput>{
  //   return this.http.get(this.configService.getApiUrl()+`getUserById/${id}`)
  // }
}
