import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginatedResult } from 'src/app/_models/pagination';
import { AddAdminDto, AdminUserForListDto } from 'src/dto/models';
import { UpdateAdminRoleDto } from 'src/dto/models/update-admin-role-dto';
import { environment } from 'src/environments/environment';

@Injectable()
export class AdminService {
  constructor(private readonly http: HttpClient) {}
  baseUrl = environment.apiUrl + 'admin/';

  addUser(user: AddAdminDto): Observable<AdminUserForListDto> {
    return this.http.post<AdminUserForListDto>(this.baseUrl, user);
  }

  updateUser(user: UpdateAdminRoleDto): Observable<AdminUserForListDto> {
    return this.http.put<AdminUserForListDto>(this.baseUrl, user);
  }

  deleteUser(userId: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + userId);
  }

  getAdmins(
    page: number,
    itemsPerPage: number,
    orderBy: string,
    sortDirection: string,
    searchString: string
  ): Observable<PaginatedResult<AdminUserForListDto[]>> {
    let params = new HttpParams();

    params = params.append('orderBy', orderBy);
    params = params.append('sortDirection', sortDirection);
    params = params.append('searchString', searchString);

    if (page != null && itemsPerPage != null) {
      params = params.append('pagenumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http
      .get<AdminUserForListDto[]>(this.baseUrl, {
        observe: 'response',
        params
      })
      .pipe(
        map(response => {
          const paginatedResult: PaginatedResult<AdminUserForListDto[]> = new PaginatedResult<AdminUserForListDto[]>();
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }
}
