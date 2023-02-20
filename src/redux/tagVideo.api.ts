import { TagVideo } from '../models/TagVideo';
import api from './api';

const tagVideoApi = api.injectEndpoints({
  endpoints: (builder) => ({
    getTagVideos: builder.query<TagVideo[], void>({
      query: () => 'tagVideo',
      providesTags: (result) => {
        return ['TagVideos'];
      }
    })
  })
});

export const { useGetTagVideosQuery } = tagVideoApi;
