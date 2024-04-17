import { Injectable } from '@angular/core';
import { IConfig } from '../../Interfaces/config/IConfig';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private config: IConfig;

  /**
   * Creates an instance of config service
   */
  constructor() { 
    const config = sessionStorage.getItem('configSettings') || '{}';
    this.config = JSON.parse(config);
  }

  /**
   * Fetches the api url
   * @returns url
   */
  public getApiUrl():string{
    return this.config.apiUrl;
  }
}
