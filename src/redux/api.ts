import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { apiUrl } from '../fetchHelper';
import { Video } from '../models/Video';
import { VideoMetadata } from '../models/VideoMetadata';

const api = createApi({
  reducerPath: 'api',
  baseQuery: fetchBaseQuery({ baseUrl: apiUrl }),
  endpoints: (builder) => ({
    getVideos: builder.query<Video[], void>({
      query: () => 'videos'
    }),
    getMetadata: builder.query<VideoMetadata[], void>({
      query: () => 'metadata'
    }),
    getMetadataForVideo: builder.query<VideoMetadata, string>({
      query: (filePath: string) => 'metadata?' + new URLSearchParams({ filePath })
    })
  })
});

export default api;

export const { useGetVideosQuery } = api;
