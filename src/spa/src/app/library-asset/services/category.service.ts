import { HttpClient } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { takeUntil, tap } from 'rxjs/operators';
import { CategoryDto } from 'src/dto/models';
import { environment } from 'src/environments/environment';

@Injectable()
export class CategoryService implements OnDestroy {
  private readonly unsubscribe = new Subject<void>();
  private categorySubject = new BehaviorSubject<CategoryDto[]>([]);
  private baseUrl = environment.apiUrl + 'category';

  categories$: Observable<CategoryDto[]> = this.categorySubject.asObservable();

  constructor(private readonly httpService: HttpClient) {
    this.loadAllCategories();
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  private loadAllCategories(): void {
    this.httpService
      .get<CategoryDto[]>(`${this.baseUrl}`)
      .pipe(
        takeUntil(this.unsubscribe),
        tap(categories => this.categorySubject.next(categories))
      )
      .subscribe();
  }
}
