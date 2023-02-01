import { Thumbnail } from '../models/Thumbnail';
import api from './api';

const thumbnailApi = api.injectEndpoints({
  endpoints: (builder) => ({
    getThumbnails: builder.query<Thumbnail[], void>({
      query: () => 'thumbnails'
    }),
    generateThumbnails: builder.mutation<
      string[],
      { filePath: string; count: number }
    >({
      query: (data) => ({
        url: 'thumbnails',
        params: data,
        method: 'GET'
      })
    })
  })
});

export const { useGenerateThumbnailsMutation, useGetThumbnailsQuery } = thumbnailApi;
