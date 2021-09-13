import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { PaginatedResult } from 'src/app/_models/pagination';
import { lmsResolverContants } from 'src/app/_resolver/resolver.constants';
import { AdminUserForListDto } from 'src/dto/models';
import { AdminService } from '../services/admin.service';

@Injectable()
export class AdminListResolver implements Resolve<Observable<PaginatedResult<AdminUserForListDto[]>>> {
  constructor(private readonly adminService: AdminService) {}
  resolve(): Observable<PaginatedResult<AdminUserForListDto[]>> {
    return this.adminService.getAdmins(lmsResolverContants.pageNumber, lmsResolverContants.pageSize, '', '', '');
  }
}
