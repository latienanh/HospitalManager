import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/administratives',
        name: '::Menu:Administrative',
        iconClass: 'fas fa-book',
        order: 2,
        layout: eLayoutType.application,
      },
      {
        path: '/provinces',
        name: '::Menu:Province',
        parentName: '::Menu:Administrative',
        layout: eLayoutType.application,
      },
      {
        path: '/districts',
        name: '::Menu:District',
        parentName: '::Menu:Administrative',
        layout: eLayoutType.application,
      },
      {
        path: '/wards',
        name: '::Menu:Ward',
        parentName: '::Menu:Administrative',
        layout: eLayoutType.application,
      },
      {
        path: '/hospitals',
        name: '::Menu:Hospital',
        iconClass: 'fas fa-hospital',
        order: 3,
        layout: eLayoutType.application,
      },
      {
        path: '/parients',
        name: '::Menu:Patient',
        iconClass: 'fas fa-hospital-user',
        order: 4,
        layout: eLayoutType.application,
      },
    
    ]);
  };
}
