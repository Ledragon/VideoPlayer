import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { apiUrl } from '../fetchHelper';
import { Video } from '../models/Video';
import { VideoMetadata } from '../models/VideoMetadata';

const api = createApi({
  reducerPath: 'api',
  baseQuery: fetchBaseQuery({ baseUrl: apiUrl }),
  tagTypes: ['Videos'],
  endpoints: (builder) => ({
    getMetadata: builder.query<VideoMetadata[], void>({
      query: () => 'metadata'
    }),
    getMetadataForVideo: builder.query<VideoMetadata, string>({
      query: (filePath: string) =>
        'metadata?' + new URLSearchParams({ filePath })
    })
  })
});

export default api;

export const {  } = api;
