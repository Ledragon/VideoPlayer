import { useSelector } from 'react-redux';

import { useGetDirectoriesQuery } from '../settings.api';
import { getTagDictionary, getTagList, useGetTagsQuery } from '../tag.api';
import { useGetTagVideosQuery } from '../tagVideo.api';
import { getVideoList, useGetVideosQuery } from '../video.api';

const useGetAllVideoInfo = () => {
  useGetVideosQuery();
  useGetTagsQuery();
  const { data: tagVideos } = useGetTagVideosQuery();

  const videos = useSelector(getVideoList);
  const tags = useSelector(getTagDictionary);
  return videos?.map((v) =>
    Object.assign(
      {},
      {
        video: v,
        tags: tagVideos?.filter((tv) => tv.videoId === v.id).map((tv) => tags[tv.tagId])
      }
    )
  );
};

export default useGetAllVideoInfo;
