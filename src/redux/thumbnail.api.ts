import { Thumbnail } from '../models/Thumbnail';
import api from './api';

const thumbnailApi = api.injectEndpoints({
  endpoints: (builder) => ({
    getThumbnails: builder.query<Thumbnail[], void>({
      query: () => 'thumbnails',
      providesTags: () => {
        return [{ type: 'Thumbnails', id: 'LIST' }];
      }
    }),
    generateThumbnails: builder.mutation<
      string[],
      { filePath: string; count: number }
    >({
      query: (data) => ({
        url: 'thumbnails/generate',
        params: data,
        method: 'GET'
      })
    }),
    addThumbnails: builder.mutation<void, Thumbnail[]>({
      query: (data) => ({
        url: 'thumbnails',
        body: data,
        method: 'POST'
      }),
      invalidatesTags: () => {
        return [{ type: 'Thumbnails', id: 'LIST' }];
      }
    }),
    deleteThumbnail: builder.mutation<Thumbnail, number>({
      query: (id) => ({
        url: 'thumbnails',
        params: { id },
        method: 'DELETE'
      }),
      invalidatesTags: () => {
        return [{ type: 'Thumbnails', id: 'LIST' }];
      }
    })
  })
});

export const {
  useGenerateThumbnailsMutation,
  useGetThumbnailsQuery,
  useAddThumbnailsMutation,
  useDeleteThumbnailMutation
} = thumbnailApi;
