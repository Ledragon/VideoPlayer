import {
  List,
  ListItem,
  ListItemButton,
  ListItemText} from '@mui/material';
import { Video } from '../../models/Video';

const VideoList = (props: {
  videos: Video[];
  onClick?: (v: Video) => void;
}) => {
  const { videos, onClick } = props;
  const handleClick = (v: Video) => {
    if (onClick) {
      onClick(v);
    }
  };
  return (
    <List>
      {videos.map((v, i) => {
        return (
          <ListItem key={i} disablePadding>
            <ListItemButton
              onClick={() => {
                handleClick(v);
              }}
            >
              <ListItemText>{v.title}</ListItemText>
            </ListItemButton>
          </ListItem>
        );
      })}
    </List>
  );
};

export default VideoList;
