import { Injectable } from '@angular/core';
import { IConfig } from '../../Interfaces/config/IConfig';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private config: IConfig;


  constructor() { 
    const config = sessionStorage.getItem('configSettings') || '{}';
    this.config = JSON.parse(config);
  }

  public getApiUrl():string{
    return this.config.apiUrl;
  }
}
