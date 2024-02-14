import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { IConfig } from './app/core/Interfaces/config/IConfig';

const config: IConfig = {
  apiUrl: 'http://localhost:5002',
};
sessionStorage.setItem('configSettings', JSON.stringify(config));
platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
